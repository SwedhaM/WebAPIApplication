using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Signup.Models;
using MimeKit;
using MailKit.Net.Smtp;

namespace Signup.Controllers;

[ApiController]
[Route("[controller]")]

public class SignupController : ControllerBase{
    private readonly IConfiguration _configuration;

    private readonly SignupDbContext _context;

    private readonly  SignupDbContext _con;

    public SignupController(IConfiguration configuration,SignupDbContext context,SignupDbContext con){
        _context=context;
        _con=con;
        _configuration = configuration;
    }
    [HttpPost]

    public async Task<ActionResult<Register>>PostEmp(Register sign)
    {
    bool emailExists = await _con.Signup_Detail.AnyAsync(s => s.Email == sign.Email);
    if (emailExists)
    {
        return BadRequest("Email already registered");
    }
    if (sign.Password != sign.Confirmpassword)
    {
        return BadRequest("Password and confirm password do not match");
    }

        _con.Signup_Detail.Add(sign);

        await _context.SaveChangesAsync();
        await _con.SaveChangesAsync();
        return (sign);
    }

    //  [HttpPost]
    //     [Route("forgotpassword")]
    //     public async Task<ActionResult<string>> ForgotPassword(string email)
    //     {
    //         var user = await _context.Signup_Detail.FirstOrDefaultAsync(u => u.Email == email);
    //         if (user == null)
    //         {
    //             return BadRequest("User not found");
    //         }

    //         var message = new MimeMessage();
    //         message.From.Add(new MailboxAddress("Swedha", "murugaiyanlatha6@gmail.com"));
    //         message.To.Add(new MailboxAddress(user.Name, user.Email));
    //         message.Subject = "Your previous password";

    //         var bodyBuilder = new BodyBuilder();
    //         bodyBuilder.TextBody = "Your previous password is: " + user.Password;

    //         message.Body = bodyBuilder.ToMessageBody();

    //         using var client = new SmtpClient();
    //         await client.ConnectAsync(_configuration["SMTP:Host"], int.Parse(_configuration["SMTP:Port"]), false);
    //         await client.AuthenticateAsync(_configuration["SMTP:Username"], _configuration["SMTP:Password"]);
    //         await client.SendAsync(message);
    //         await client.DisconnectAsync(true);

    //         return Ok();
    //     }
    [HttpPost]
[Route("forgotpassword")]
public async Task<ActionResult<string>> ForgotPassword(ForgotPasswordRequest request)
{
    var user = await _context.Signup_Detail.FirstOrDefaultAsync(u => u.Email == request.Email);
    if (user == null)
    {
        return BadRequest("User not found");
    }

    var message = new MimeMessage();
    message.From.Add(new MailboxAddress("Sw...", "murugaiyanlatha6@gmail.com"));
    message.To.Add(new MailboxAddress("", user.Email));
    message.Subject = "Password reset request";
    message.Body = new TextPart("plain")
    {
        Text = "Please click on the following link to reset your password: http://localhost:5285/resetpassword?email=" + user.Password
    };

    using (var client = new SmtpClient())
    {
        client.Connect(_configuration["SMTP:Host"], int.Parse(_configuration["SMTP:Port"]), false);
        client.Authenticate(_configuration["SMTP:Username"], _configuration["SMTP:Password"]);
        await client.SendAsync(message);
        client.Disconnect(true);
    }

    return Ok("Password reset email has been sent");
}

public class ForgotPasswordRequest
{
    public string Email { get; set; }
}

    }
