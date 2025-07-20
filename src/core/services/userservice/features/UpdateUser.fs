namespace core.Services.UserService

open System
open core.Records

module UpdateUser =
  let IsRequestValid (request : User) =
    request.Id > 0
    request.Handle <> String.Empty &&
    not (request.Handle |> String.forall Char.IsWhiteSpace)

  let MapUserDeltaToExistingUser userDelta existingUser =
    { existingUser with
        Handle = userDelta.Handle
    }
