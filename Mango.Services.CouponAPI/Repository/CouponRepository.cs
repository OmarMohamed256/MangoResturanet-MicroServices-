using AutoMapper;
using Mango.Services.CouponAPI.DbContexts;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public CouponRepository(ApplicationDbContext db, IMapper mapper)
        {
            _mapper= mapper;
            _db = db;
        }

        public async Task<CouponDto> GetCouponByCode(string couponCode)
        {
            var couponFromDb = await _db.Coupons.FirstOrDefaultAsync(c => c.CouponCode == couponCode);
            return _mapper.Map<CouponDto>(couponFromDb);
        }
    }
}
