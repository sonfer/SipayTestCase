using MassTransit;
using Microsoft.Extensions.Logging;
using SipayTestCase.Application.Interfaces;
using SipayTestCase.Contract.Messages;

namespace SipayTestCase.Mail.Consumers;

public class RegisterMailConsumer:  IConsumer<UserRegisterMessage>
{
    private readonly ILogger<RegisterMailConsumer> _logger;
    private readonly IUserRepository _userRepository;

    public RegisterMailConsumer(ILogger<RegisterMailConsumer> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task Consume(ConsumeContext<UserRegisterMessage> context)
    {
        if (context.Message == null)
            throw new Exception("context.Message is null");
        
        
        
        _logger.LogInformation($"{context.Message.UserId} id'li kullanıcı için  mail servisi başladı");

        var user = await _userRepository.GetAsync(d => d.Id == context.Message.UserId);
        if (user!= null)
        {
            //Kullanıcı için aktivasyon kodunun mail ile gönderilmesi işlemleri.
            
            
            var activationCode = Guid.NewGuid();
            user.ActivationCode = activationCode.ToString();

            await _userRepository.UpdateAsync(user);
            
            _logger.LogInformation($"https://localhost:3001/user/activationCode={user.ActivationCode}");
        }
        _logger.LogInformation($"{context.Message.UserId} id'li kullanıcı için mail gönderimi tamamlandı");
    }
}