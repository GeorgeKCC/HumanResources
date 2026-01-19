using ColaboratorContract.Dtos.Request;

namespace ColaboratorContract.Contracts
{
    public interface IAskWithRagColaborator
    {
        Task<string> Ask(AskQuestionColaboratorRequest askQuestionColaboratorRequest);
    }
}
