defmodule Wallace.Schemas.Category do
  use Ecto.Schema
  import Ecto.Changeset

  schema "categories" do
    field :icon, :string
    field :name, :string

    timestamps()
  end

  @doc false
  def changeset(category, attrs) do
    category
    |> cast(attrs, [:name, :icon])
    |> validate_required([:name, :icon])
  end
end
