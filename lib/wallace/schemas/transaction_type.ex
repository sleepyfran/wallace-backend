defmodule Wallace.Schemas.TransactionType do
  use Ecto.Schema
  import Ecto.Changeset

  schema "transaction_types" do
    field :name, :string

    timestamps()
  end

  @doc false
  def changeset(transaction_type, attrs) do
    transaction_type
    |> cast(attrs, [:name])
    |> validate_required([:name])
  end
end
