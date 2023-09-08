# MangoResturanet-MicroServices-
.NET Core Microservices - The Complete Guide
# Restaurant Application
# Overview
This restaurant application is built using microservices architecture in ASP.NET, utilizing Azure Service Bus for seamless communication between services.

# Technologies Used
ASP.NET Core
Azure Service Bus
Entity Framework Core
Swagger/OpenAPI
SQL Server


# Microservices
1. Message Bus Service
Utilizes Azure Service Bus for reliable messaging between microservices.
Topics/Subscriptions are utilized for asynchronous communication.
2. Coupon API
Manages coupon creation, validation, and redemption.
Exposes RESTful APIs for coupon management.
3. Email Service
Responsible for sending transactional emails, order confirmations, etc.
Utilizes SMTP protocols and templates for email delivery.
4. Identity Service
Handles user authentication and authorization.
Integrates with other services for user-specific functionality.
5. Order API
Manages order processing, including creation and status updates.
Communicates with other services for item availability, coupon validation, and payment processing.
6. Payment API
Manages payment processing, including authorization and capture.
Integrates with the Order API for payment validation and completion.
7. Product API
Manages product information, including creation, retrieval, and modification.
Exposes RESTful APIs for product management.
8. Shopping Cart API
Handles shopping cart functionality, including item addition, removal, and order creation.
Communicates with other services for pricing and availability.
