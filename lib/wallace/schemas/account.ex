defmodule Wallace.Schemas.Account do
  use Ecto.Schema
  use Wallace.Schemas.Base

  import Ecto.Changeset

  alias Wallace.Schemas.AccountType
  alias Wallace.Schemas.User

  schema "accounts" do
    field :balance, Money.Ecto.Composite.Type
    field :name, :string

    belongs_to :type, AccountType
    belongs_to :user, User

    timestamps()
  end

  @doc false
  def changeset(account, attrs) do
    account
    |> cast(attrs, [:name, :balance, :type_id])
    |> validate_required([:name, :balance, :type_id])
  end
end
