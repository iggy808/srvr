namespace Core.AsyncTools
open System.Threading.Tasks

module TaskTools =
  let await (t : Task) = t |> Async.AwaitIAsyncResult |> Async.Ignore
