defmodule Wallace.Repo do
  use Ecto.Repo,
    otp_app: :wallace,
    adapter: Ecto.Adapters.Postgres
end
