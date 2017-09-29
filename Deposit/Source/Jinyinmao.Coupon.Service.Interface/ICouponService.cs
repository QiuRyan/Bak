using System.Threading.Tasks;
using Jinyinmao.Deposit.Domain;
using Jinyinmao.Tirisfal.Service.Interface.Dtos;

namespace Jinyinmao.Coupon.Service.Interface
{
    public interface ICouponService
    {
        Task<BasicResult<string>> DouDiIncreaseRateCouponAsync(DouDiCouponMessage request);

        Task<BasicResult<string>> UseCashBackCouponAsync(UseCouponMessage request);

        Task<BasicResult<string>> UseIncreaseCouponAsync(UseCouponMessage request);

        Task<BasicResult<string>> UsePrincipalCouponAsync(UseCouponMessage request);
    }
}