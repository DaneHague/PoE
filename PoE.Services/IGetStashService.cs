using PoE.Services.Models;

namespace PoE.Services;

public interface IGetStashService
{
    Task<Stash> GetAllStashTabs();
}