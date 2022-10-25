using Midas.Services.Family;
using Midas.Services.User;

namespace Application.Helpers;

public static class PermissionHelper
{
    public static async Task<bool> IsTransactionOwnedByUserOrHisFamily(TransactionOwnedByUserOrHisFamilyArgs args)
    {
        var activeUser = await args.UserClient.GetActiveUserAsync().ConfigureAwait(false);
        var userFamilyRoles = await args.FamilyClient.GetFamilyMembershipsForUserAsync().ConfigureAwait(false);
        var canChangeValue = new List<bool>
        {
            activeUser.Id == args.TransactionId,
            userFamilyRoles.Items.FirstOrDefault(x => x.User.Id == activeUser.Id).FamilyRole.Name ==
            "Main administrator"
        };

        return !canChangeValue.All(x => x);
    }
}

public class TransactionOwnedByUserOrHisFamilyArgs
{
    public long TransactionId { get; set; }
    public IUserClient UserClient { get; set; }
    public IFamilyClient FamilyClient { get; set; }
}