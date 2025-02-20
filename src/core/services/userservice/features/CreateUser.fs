namespace core.Services.UserService.Features
open System
open core.Records

module CreateUser =
  module CreateUserValidation =
    let private UserIdIsNotEmpty userId = userId <> Guid.Empty
    let private HandleIsNotEmpty handle = handle <> String.Empty
    let private HandleIsNotWhitespace handle = handle |> String.forall Char.IsWhiteSpace

    let IsRequestValid (request : User) =
      UserIdIsNotEmpty request.Id &&
      HandleIsNotEmpty request.Handle &&
      HandleIsNotWhitespace request.Handle
