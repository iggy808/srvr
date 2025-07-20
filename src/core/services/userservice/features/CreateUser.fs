namespace core.Services.UserService
open System
open core.Records

module CreateUser =
  let IsRequestValid (request : User) =
    request.Handle <> String.Empty &&
    not (request.Handle |> String.forall Char.IsWhiteSpace)
