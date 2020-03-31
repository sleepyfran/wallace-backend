defmodule Wallace.Schemas.Account do
  use Ecto.Schema
  import Ecto.Changeset

  alias Wallace.Schemas.AccountType

  schema "accounts" do
    field :balance, Money.Ecto.Composite.Type
    field :name, :string

    belongs_to :type, AccountType

    timestamps()
  end

  @doc false
  def changeset(account, attrs) do
    account
    |> cast(attrs, [:name, :balance])
    |> validate_required([:name, :balance])
  end
end
