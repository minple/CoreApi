using CoreApi.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
public class TokenBlackList1 {
    public bool CheckToken(CoreApiContext context, string token) {
        bool Respone;
        var Result = context.TokenBlackList.Where(tk => tk.Token == token);
        int Count = 0;
        foreach(var c in Result) {
            Count++;
        }
        if(Count > 1) {
            Respone = true;
        }
        else
            Respone = false;
        return Respone;
    }
}