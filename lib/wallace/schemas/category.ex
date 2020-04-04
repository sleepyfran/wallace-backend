defmodule Wallace.Schemas.Category do
  use Ecto.Schema
  use Wallace.Schemas.Base

  import Ecto.Changeset

  alias Wallace.Schemas.User

  schema "categories" do
    field :icon, :string
    field :name, :string

    belongs_to :user, User

    timestamps()
  end

  @doc false
  def changeset(category, attrs) do
    category
    |> cast(attrs, [:name, :icon])
    |> validate_required([:name, :icon])
  end
end
