using Application.Dto;
using Midas.Services.Family;
using Midas.Services.User;

namespace Application.Helpers;

public static class PermissionHelper
{
    public static async Task<bool> IsTransactionOwnedByUserOrHisFamily(TransactionOwnedByUserOrHisFamilyArgs args)
    {
        var activeUser = await args.UserClient.GetActiveUserAsync().ConfigureAwait(false);
        var userFamilyRoles = await args.FamilyClient.GetFamilyMembershipsForUserAsync().ConfigureAwait(false);

        if (activeUser.Id == (long)args.Transaction.UserId)
        {
            return true;
        }
        
        if (userFamilyRoles.Count > 0)
        {
            return userFamilyRoles.Items.FirstOrDefault(x => x.User.Id == activeUser.Id).FamilyRole.Name ==
                "Main administrator";
        }

        return false;
    }
}

public class TransactionOwnedByUserOrHisFamilyArgs
{
    public TransactionDto Transaction { get; set; }
    public IUserClient UserClient { get; set; }
    public IFamilyClient FamilyClient { get; set; }
}