namespace KunaV2.Model
{
    /*
    * 2002: Failed to create order.
    * 2006: The tonce has already been used by access key.
    * 2007: The tonce is invalid, current timestamp is 1597600013000
    */

    public class ErrorModel
    {
        public Error Error { get; set; }
    }

    public class Error
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }

    public enum ErrorCode
    {
        FailedToCreateOrder = 2002,
        TonceAlreadyUsed = 2006,
        TonceInvalid = 2007
    }
}
