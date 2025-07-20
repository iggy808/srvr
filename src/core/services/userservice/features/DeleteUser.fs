namespace core.Services.UserService
open System

module DeleteUser =
  let IsRequestValid (request : int) =
    request > 0