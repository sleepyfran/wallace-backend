using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Wallace.Application.Commands.Setup
{
    public enum CategorySelection
    {
        Default,
        Scratch
    }
    
    /// <summary>
    /// Input given by the user when he finished the setup process.
    /// </summary>
    public class SetupCommand : IRequest<int>
    {
        /// <summary>
        /// Name assigned to the account.
        /// </summary>
        public string AccountName { get; set; }
        
        /// <summary>
        /// Current balance of the account,
        /// </summary>
        public decimal InitialBalance { get; set; }
        
        /// <summary>
        /// Currency code. Ex: EUR, CZK, USD, etc.
        /// </summary>
        public string BaseCurrency { get; set; }
        
        /// <summary>
        /// Whether the user decided to use the default categories or to
        /// remove everything and start from scratch.
        /// </summary>
        public CategorySelection CategorySelection { get; set; }
        
        /// <summary>
        /// Email of the user used to login.
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Password as inputted by the user.
        /// </summary>
        public string Password { get; set; }
    }
    
    /// <summary>
    /// Setups the user's profile and creates a basic first account with the
    /// selected base currency.
    /// </summary>
    public class SetupCommandHandler : IRequestHandler<SetupCommand, int>
    {
        public Task<int> Handle(
            SetupCommand request, 
            CancellationToken cancellationToken
        )
        {
            throw new System.NotImplementedException();
        }
    }
}