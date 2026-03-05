using Microsoft.AspNetCore.Mvc;
using ParcelCommon;
using ParcelCommon.Interfaces;
using ParcelCommon.Utilities;
using ParcelCreate.DTOs;
using ParcelCreate.Interfaces;
using ParcelCreate.Services;
using ParcelCreate.Utilities;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBookingSvc, BookingSvc> ();
builder.Services.AddScoped<IParcelScanSvc, ParcelScanSvc> ();

builder.Services.Configure<EHSettings>(builder.Configuration.GetSection("EHSettings"));
builder.Services.Configure<CDBSettings>(builder.Configuration.GetSection("CDBSettings"));
builder.Services.AddScoped<IEHManager, EHManager>();


builder.Services.AddAuthentication();
builder.Services.AddAuthentication("Bearer").AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddHttpClient("ParcelAPIBackend", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration.GetValue<String>("ParcelAPIBackend"));
    httpClient.DefaultRequestHeaders.Accept.Clear();
    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseAuthentication();
    app.UseAuthorization();
}

app.UseHttpsRedirection();

//app.MapGet("/secret", (ClaimsPrincipal user) => $"Hello {user.Identity?.Name}. My secret")
//    .RequireAuthorization();

app.MapPost("/parcel/new", async (BookingRequestDto bookingInfoReq, IBookingSvc _bookingSvc) => 
{
    IBookingSvc bookingSvc = _bookingSvc;
    var booking = await bookingSvc.CreateNewBooking(bookingInfoReq);

    if(booking != null && booking.ShipmentConfirmed)
    {
        return Results.Ok(booking);
    }

    return Results.BadRequest();
})
    .RequireAuthorization()
    .WithName("BookingRequest")
    .WithOpenApi();

app.MapPost("/parcel/scan", async (ParcelScanRequestDto parcelScanInfoReq, IParcelScanSvc _parcelScanSvc) =>
{
    IParcelScanSvc parcelScanSvc = _parcelScanSvc;
    var scanStatus = await parcelScanSvc.CreateScanEvent(parcelScanInfoReq);

    if (scanStatus != null && scanStatus.ScanEventSucess)
    {
        return Results.Ok(scanStatus);
    }
    return Results.BadRequest();
})
    .RequireAuthorization()
    .WithName("ScanEvent")
    .WithOpenApi();

app.MapGet("/parcel/track/{trackingId:long}", async (long trackingId, IParcelScanSvc _parcelScanSvc) =>
{
    IParcelScanSvc parcelScanSvc = _parcelScanSvc;
    var trackingStatus = await parcelScanSvc.TrackParcelStatus(trackingId);

    if (trackingStatus != null)
    {
        return Results.Ok(trackingStatus);
    }
    return Results.BadRequest();
})
    .WithName("TrackEvent")
    .WithOpenApi();

app.Run();


