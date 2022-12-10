using Azure.Messaging.ServiceBus;
using Mango.Services.OrderAPI.Messages;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Repository;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.OrderAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string subscriptionCheckout;
        private readonly string checkoutMessageTopic;
        private readonly OrderRepository _orderRepository;

        private ServiceBusProcessor checkOutProcessor;

        private readonly IConfiguration _configuration;
        public AzureServiceBusConsumer(OrderRepository orderRepository, IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _configuration = configuration;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            subscriptionCheckout = _configuration.GetValue<string>("SubscriptionCheckout");
            checkoutMessageTopic = _configuration.GetValue<string>("CheckoutMessageTopic");

            var client = new ServiceBusClient(serviceBusConnectionString);

            checkOutProcessor = client.CreateProcessor(checkoutMessageTopic, subscriptionCheckout);
        }

        public async Task Start()
        {
            checkOutProcessor.ProcessMessageAsync += OnCheckoutMessageRecived;
            checkOutProcessor.ProcessErrorAsync += ErrorHandler;
            await checkOutProcessor.StartProcessingAsync();
        }
        public async Task Stop()
        {
            await checkOutProcessor.StopProcessingAsync();
            await checkOutProcessor.DisposeAsync();
        }
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnCheckoutMessageRecived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CheckoutHeaderDto checkoutHeaderDto = JsonConvert.DeserializeObject<CheckoutHeaderDto>(body);

            OrderHeader orderHeader = new OrderHeader()
            {
                UserId = checkoutHeaderDto.UserId,
                FirstName= checkoutHeaderDto.FirstName,
                LastName= checkoutHeaderDto.LastName,
                OrderDetails= new List<OrderDetails>(),
                CardNumber= checkoutHeaderDto.CardNumber,
                CouponCode= checkoutHeaderDto.CouponCode,
                DiscountTotal= checkoutHeaderDto.DiscountTotal,
                Email= checkoutHeaderDto.Email,
                OrderTime = DateTime.Now,
                OrderTotal= checkoutHeaderDto.OrderTotal,
                PaymentStatus = false,
                Phone= checkoutHeaderDto.Phone,
                PickupDateTime= checkoutHeaderDto.PickupDateTime,
                CVV = checkoutHeaderDto.CVV,
                ExpiryMonthYear= checkoutHeaderDto.ExpiryMonthYear,

            };
            foreach(var detailList in checkoutHeaderDto.CartDetails)
            {
                OrderDetails orderDetails = new OrderDetails()
                {
                    ProductId= detailList.ProductId,
                    ProductName=detailList.Product.Name,
                    Price = detailList.Product.Price,
                    Count = detailList.Count
                };
                orderHeader.CartTotalItems += detailList.Count;
                orderHeader.OrderDetails.Add(orderDetails);
            }

            await _orderRepository.AddOrder(orderHeader);
        }
    }
}
