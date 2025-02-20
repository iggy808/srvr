namespace core.Services.UserService
open System

module GetUserById =
  module Validation =
    let private UserIdIsNotEmpty userId = userId <> Guid.Empty

    let IsRequestValid request =
      UserIdIsNotEmpty request
