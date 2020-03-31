defmodule Wallace.Schemas.Transaction do
  use Ecto.Schema
  import Ecto.Changeset

  alias Wallace.Schemas.Account
  alias Wallace.Schemas.Category
  alias Wallace.Schemas.Payee
  alias Wallace.Schemas.TransactionType
  alias Wallace.Schemas.Types.Repetition

  schema "transactions" do
    field :amount, Money.Ecto.Composite.Type
    field :date, :utc_datetime
    field :notes, :string
    field :repetition, Repetition

    belongs_to :account, Account
    belongs_to :category, Category
    belongs_to :payee, Payee
    belongs_to :type, TransactionType

    timestamps()
  end

  @doc false
  def changeset(transaction, attrs) do
    transaction
    |> cast(attrs, [:amount, :date, :notes])
    |> validate_required([:amount, :date, :notes])
  end
end
