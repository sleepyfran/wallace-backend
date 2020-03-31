defmodule Wallace.Schemas.AccountType do
  use Ecto.Schema
  import Ecto.Changeset

  schema "account_types" do
    field :name, :string

    timestamps()
  end

  @doc false
  def changeset(account_type, attrs) do
    account_type
    |> cast(attrs, [:name])
    |> validate_required([:name])
  end
end
