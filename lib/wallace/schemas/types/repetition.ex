defmodule Wallace.Schemas.Types.Repetition do
  use Exnumerator, values: [:never, :daily, :weekly, :monthly, :biannually, :annually]
end
