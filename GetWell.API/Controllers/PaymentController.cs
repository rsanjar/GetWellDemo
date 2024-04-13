using System.Threading.Tasks;
using GetWell.API.Models;
using GetWell.Core.Models.Payment.Request;
using GetWell.Core.Models.Payment.Request.Methods;
using GetWell.Core.Models.Payment.Response;
using GetWell.Core.Models.Payment.Response.Methods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace GetWell.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    #region ctor

    private readonly RestClient _restClient;
    private readonly HeaderParameter _xAuth;
    private readonly HeaderParameter _xAuthWithSecretKey;

    public PaymentController(PaymentConfig paymentConfig)
    {
        _restClient = new RestClient(paymentConfig.BaseUrl);
        _xAuth = new HeaderParameter("X-Auth", $"{paymentConfig.AuthKey}");
        _xAuthWithSecretKey = new HeaderParameter("X-Auth", $"{paymentConfig.AuthKey}:{paymentConfig.AuthSecretKey}");
    }

    #endregion

    [AllowAnonymous]
    [HttpPost("create-card")]
    public async Task<ActionResult<RootResponse<CardCreateResponse>>> CreateCard(CardCreateRequest request)
    {
        var response = await MakeRequest<CardCreateResponse>(request);
        
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("get-card-verify-code")]
    public async Task<ActionResult<RootResponse<CardVerifyCodeResponse>>> GetCardVerifyCode(CardVerifyCodeRequest request)
    {
        var response = await MakeRequest<CardVerifyCodeResponse>(request);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("card-verify")]
    public async Task<ActionResult<RootResponse<CardVerifyResponse>>> CardVerify(CardVerifyRequest request)
    {
        var response = await MakeRequest<CardVerifyResponse>(request);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("card-check")]
    public async Task<ActionResult<RootResponse<CardCheckResponse>>> CardCheck(CardCheckRequest request)
    {
        var response = await MakeRequest<CardCheckResponse>(request, true);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("card-remove")]
    public async Task<ActionResult<RootResponse<CardRemoveResponse>>> CardRemove(CardRemoveRequest request)
    {
        var response = await MakeRequest<CardRemoveResponse>(request, true);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("receipt-create")]
    public async Task<ActionResult<RootResponse<ReceiptCreateResponse>>> ReceiptCreate(ReceiptCreateRequest request)
    {
        var response = await MakeRequest<ReceiptCreateResponse>(request, true);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("receipt-pay")]
    public async Task<ActionResult<RootResponse<ReceiptPayResponse>>> ReceiptPay(ReceiptPayRequest request)
    {
        var response = await MakeRequest<ReceiptPayResponse>(request, true);

        return Ok(response);
    }
    

    #region Private Methods

    private async Task<RootResponse<TResult>> MakeRequest<TResult>(IBaseRequest<IResponseBase> request, bool isFullAuth = false)
        where TResult : IResult
    {
        var requestObject = new RestRequest<IBaseRequest<IResponseBase>, RootResponse<TResult>>(_restClient, request);
        
        var response = await requestObject.RequestAsync(headerParameters: isFullAuth ? _xAuthWithSecretKey : _xAuth);

        return response;
    }

    #endregion

}