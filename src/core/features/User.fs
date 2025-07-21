namespace core.features

open System
open System.Threading.Tasks
open core.records
open core.repositories

module User =
  type Features = {
    GetUser : int -> Task<Option<User>>
    CreateUser : User -> Task<unit>
    UpdateUser : User -> Task<unit>
    DeleteUser : int -> Task<unit>
  }

  let getUser (userRepository : IUserRepository) (userId : int) = task {
    return! userRepository.GetUserById userId
  }

  let createUser (userRepository : IUserRepository) (user : User) = task {
    do! userRepository.CreateUser user
  }

  let updateUser (userRepository : IUserRepository) (user : User) = task {
    let! existingUser = userRepository.GetUserById user.Id
    match existingUser with
    | None -> failwith "handle later"
    | Some u ->
      let updatedUser = {u with Handle = user.Handle}
      do! userRepository.UpdateUser updatedUser
  }

  let deleteUser (userRepository : IUserRepository) (userId : int) = task {
    do! userRepository.DeleteUserById userId
  }

  module Validation =
    let getUserRequestIsValid (userId : int) =
      userId > 0

    let createUserRequestIsValid (user : User) =
      user.Id > 0 &&
      user.Handle <> String.Empty &&
      not (user.Handle |> String.forall Char.IsWhiteSpace)

    let updateUserRequestIsValid (user : User) =
      user.Handle <> String.Empty &&
      not (user.Handle |> String.forall Char.IsWhiteSpace)

    let deleteUserRequestIsValid (userId : int) =
      userId > 0