using Core.Constants;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = DefinedPolicy.DefaultPolicy)]

public class ImageController : ControllerBase
{
    private readonly IImageService _imageService;

    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }

    //[HttpGet("original/{imageId}")]
    //public async Task<IActionResult> GetOriginalImageAsync(Guid imageId)
    //{
    //    var result = await _imageService.GetOriginalImageByIdAsync(imageId);

    //    if (!result.IsSuccess)
    //        return StatusCode(result.Error!.StatusCode, result.Error!.Message);

    //    return Ok(result.Value);
    //}

    [HttpGet("profile/{imageId}")]
    public async Task<IActionResult> GetProfileImageAsync(Guid imageId)
    {
        var result = await _imageService.GetProfileAsync(imageId);

        if (!result.IsSuccess)
            return StatusCode(result.Error!.StatusCode, result.Error!.Message);

        return Ok(result.Value);
    }

    [HttpGet("thumbnail/{imageId}")]
    public async Task<IActionResult> GetThumbnailImageAsync(Guid imageId)
    {
        var result = await _imageService.GetThumbnailAsync(imageId);

        if (!result.IsSuccess)
            return StatusCode(result.Error!.StatusCode, result.Error!.Message);

        return Ok(result.Value);
    }
}
