namespace Core.Services.UserService
open System
open Core.Records

module UpdateUser =

  module Validation =
    let private UserIdIsNotEmpty userId = userId <> Guid.Empty
    let private HandleIsNotEmpty handle = handle <> String.Empty
    let private HandleIsNotWhitespace handle = handle |> String.forall Char.IsWhiteSpace

    let IsRequestValid (request : User) =
      UserIdIsNotEmpty request.Id &&
      HandleIsNotEmpty request.Handle &&
      HandleIsNotWhitespace request.Handle

  module Logic =
    let MapUserDeltaToExistingUser userDelta existingUser = {existingUser with
      Handle = userDelta.Handle
    }
