using GetWell.Core.Enums;
using GetWell.Core.Models.Payment.Response;

namespace GetWell.Core.Models.Payment.Request.Methods;

public class CardRemoveRequest : CardTokenItem, IBaseRequest<IResponseBase>
{
    public string MethodName => PaymentMethods.CardRemove;
}