using MediatR;
using Microsoft.AspNetCore.Http;
using SipayTestCase.Application.Interfaces;
using SipayTestCase.Shared.Helpers;
using SipayTestCase.Shared.Responses;
using IResult = SipayTestCase.Shared.Interfaces.IResult;

namespace SipayTestCase.Application.Cqrs.Commands.User;

public class UserUpdateCommand: IRequest<IResult>
{
    public string Email { get; set; }
    public string FullName { get; set; }
    public string Password { get; set; }
    
    public class UserUpdateCommandHandler: IRequestHandler<UserUpdateCommand, IResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserUpdateCommandHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IResult> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.GetUserId();
            if (!userId.HasValue)
                return new ErrorResult("Kullanıcı kimliği tespit edilemedi. Lütfen yeniden oturum açınız");

            var user = await _userRepository.GetAsync(d => d.Id == userId.Value);
            if(user==null)
                return new ErrorResult("Kullanıcı bulunamadı.");

            user.FullName = request.FullName;
            user.Email = request.Email;
            
            byte[] passwordHash, passwordSalt;
            Helpers.CreatePasswordHash(request.Password!, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _userRepository.UpdateAsync(user);

            return new SuccessResult("Kullanıcı bilgileriniz başarılı olarak güncellenmiştir.");
        }
    }
}