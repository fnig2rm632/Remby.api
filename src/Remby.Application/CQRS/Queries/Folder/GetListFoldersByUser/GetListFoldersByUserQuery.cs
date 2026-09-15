using MediatR;
using Remby.Application.CQRS.Responses.Folder;
using Remby.Domain.Common;

namespace Remby.Application.CQRS.Queries.Folder.GetListFoldersByUser;

public record GetListFoldersByUserQuery(string UserId) : IRequest<Result<List<FolderShortResponse>>>;
