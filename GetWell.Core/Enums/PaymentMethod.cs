namespace GetWell.Core.Enums;

public static class PaymentMethods
{
    public const string CardCreate = "cards.create";
    public const string CardGetVerifyCode = "cards.get_verify_code";
    public const string CardVerify = "cards.verify";
    public const string CardCheck = "cards.check";
    public const string CardRemove = "cards.remove";
    public const string ReceiptCreate = "receipts.create";
    public const string ReceiptsConfirmHold = "receipts.confirm_hold";
    public const string ReceiptCancel = "receipts.cancel";
    public const string ReceiptPay = "receipts.pay";
    public const string ReceiptSend = "receipts.send";
    public const string ReceiptCheck = "receipts.check";
    public const string ReceiptGet = "receipts.get";
    public const string ReceiptGetAll = "receipts.get_all";
    public const string ReceiptSetFiscalData = "receipts.set_fiscal_data";
}