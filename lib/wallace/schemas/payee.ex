defmodule Wallace.Schemas.Payee do
  use Ecto.Schema
  import Ecto.Changeset

  schema "payees" do
    field :name, :string

    timestamps()
  end

  @doc false
  def changeset(payee, attrs) do
    payee
    |> cast(attrs, [:name])
    |> validate_required([:name])
  end
end
