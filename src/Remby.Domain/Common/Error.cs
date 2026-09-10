namespace Remby.Domain.Common;

public record Error(string Message, ErrorType ErrorType)
{
    public static Error None => new Error(string.Empty, ErrorType.None);
    
    private static Error Validation(string message = "Invalid data format") => new(message, ErrorType.Validation);
    private static Error NotFound(string message = "Resource not found") => new(message, ErrorType.NotFound);
    private static Error BusinessRule(string message = "Broken business rule") => new(message, ErrorType.BusinessRule);  
    private static Error Conflict(string message = "Conflict data") => new(message, ErrorType.Conflict);  
    private static Error Internal(string message = "Something went wrong") => new(message, ErrorType.Internal);
    private static Error Unavailable(string massage = "Service Unavailable") => new(massage, ErrorType.Unavailable);
    private static Error Timeout(string massage = "Gateway Timeout") => new(massage, ErrorType.Timeout);

    public static class User
    {
        public static Error EmptyLogin => Validation("User cannot have empty Login");
        public static Error DateVisitInFuture => Validation("User cannot have date in future");
    }

    public static class Folder
    {
        public static Error EmptyName => Validation("Folder cannot have empty Name");
        public static Error NameTooLong(int maxLength) => 
            Validation($"Folder name cannot exceed {maxLength} characters");
        public static Error DescriptionTooLong(int maxLength) => 
            Validation($"Description cannot exceed {maxLength} characters");
        public static Error DataInFuture => Validation("Date delete cannot be in future");
    }

    public static class Card
    {
        public static Error EmptyTitle => Validation("Card cannot have empty title");
        public static Error TitleTooLong(int maxLength) => 
            Validation($"Title name cannot exceed {maxLength} characters");
        public static Error HintTooLong(int maxLength) => 
            Validation($"Card hint cannot exceed {maxLength} characters");
        public static Error EmptyDecision => Validation("Card cannot have empty decision");
        public static Error DecisionTooLong(int maxLength) => 
            Validation($"Decision cannot exceed {maxLength} characters");
        public static Error LastRepeatInFuture => Validation("Last repeat cannot be in future");
        public static Error DeleteAtInFuture => Validation("DeleteAt cannot be in future");
    }
    
    public static class Database
    {
        public static Error ConnectionFailed => Unavailable("Connection to server failed");
        public static Error TimeoutGateway => Timeout("Timeout connection to server");
        public static Error InternalServer => Internal("Internal Server Error");
    }

    public static class Rank
    {
        public static Error EmptyId => Validation("Rank cannot have empty id");
        public static Error EmptyName => Validation("Rank cannot have empty name");
        public static Error MinDate => Validation("Rank cannot have min date");
    }
    
}

public enum ErrorType
{
    None = 000,
    Validation = 400,
    NotFound = 	404,
    BusinessRule = 422,
    Conflict = 409,
    Unavailable = 503,
    Internal = 500,
    Timeout = 504
}