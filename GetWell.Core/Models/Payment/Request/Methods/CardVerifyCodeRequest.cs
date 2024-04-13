using GetWell.Core.Enums;
using GetWell.Core.Models.Payment.Response;
using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment.Request.Methods;

public class CardVerifyCodeRequest : CardTokenItem, IBaseRequest<IResponseBase>
{
    public string MethodName => PaymentMethods.CardGetVerifyCode;
}