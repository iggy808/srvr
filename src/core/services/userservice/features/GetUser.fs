namespace core.Services.UserService
open System

module GetUser =
  let IsRequestValid (request : int) =
    request > 0
