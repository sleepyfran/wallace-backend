# This file is responsible for configuring your application
# and its dependencies with the aid of the Mix.Config module.
#
# This configuration file is loaded before any dependency and
# is restricted to this project.

# General application configuration
use Mix.Config

config :wallace,
  ecto_repos: [Wallace.Repo]

# Configures the endpoint
config :wallace, WallaceWeb.Endpoint,
  url: [host: "localhost"],
  secret_key_base: "XSwMS6JTAMv1JM2/+En2sj68HA8CGOsX4hLOaqgIp3lUIqWm6BzTR4w1HZLeYxMk",
  render_errors: [view: WallaceWeb.ErrorView, accepts: ~w(json)],
  pubsub: [name: Wallace.PubSub, adapter: Phoenix.PubSub.PG2],
  live_view: [signing_salt: "PVwzrn+r"]

# Configures Elixir's Logger
config :logger, :console,
  format: "$time $metadata[$level] $message\n",
  metadata: [:request_id]

# Use Jason for JSON parsing in Phoenix
config :phoenix, :json_library, Jason

# Import environment specific config. This must remain at the bottom
# of this file so it overrides the configuration defined above.
import_config "#{Mix.env()}.exs"
