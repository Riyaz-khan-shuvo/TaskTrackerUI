using AutoMapper;

namespace TaskTracker.Shared.Contracts
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile);
    }
}
