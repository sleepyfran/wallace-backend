defmodule WallaceWeb.Router do
  use WallaceWeb, :router

  pipeline :api do
    plug :accepts, ["json"]
  end

  scope "/api", WallaceWeb do
    pipe_through :api
  end
end
