defmodule Wallace.Repo.Migrations.MoneyType do
  use Ecto.Migration

  def up do
    execute """
    CREATE TYPE public.money_type AS (amount integer, currency char(3))
    """
  end

  def down do
    execute """
    DROP TYPE public.money_type
    """
  end
end
