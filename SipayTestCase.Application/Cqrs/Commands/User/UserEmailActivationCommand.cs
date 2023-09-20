using MediatR;
using SipayTestCase.Application.Interfaces;
using SipayTestCase.Shared.Interfaces;
using SipayTestCase.Shared.Responses;

namespace SipayTestCase.Application.Cqrs.Commands.User;

public class UserEmailActivationCommand:  IRequest<IResult>
{
    public string Code { get; set; }
    
    public class UserEmailActivationCommandHandler: IRequestHandler<UserEmailActivationCommand, IResult>
    {
        private readonly IUserRepository _userRepository;

        public UserEmailActivationCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IResult> Handle(UserEmailActivationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(d => d.ActivationCode == request.Code);
            if (user == null)
                return new ErrorResult("Kod doğrulanamadı");

            user.IsValidUser = true;

            await _userRepository.UpdateAsync(user);

            return new SuccessResult("Aktivasyon işleminiz tamamlanmıitır. Lütfen oturum açınız");
        }
    }
}