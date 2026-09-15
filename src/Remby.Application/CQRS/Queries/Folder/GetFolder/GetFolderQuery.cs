using MediatR;
using Remby.Application.CQRS.Responses.Folder;
using Remby.Domain.Common;

namespace Remby.Application.CQRS.Queries.Folder.GetFolder;

public record GetFolderQuery(int Id) : IRequest<Result<FolderResponse>>;
