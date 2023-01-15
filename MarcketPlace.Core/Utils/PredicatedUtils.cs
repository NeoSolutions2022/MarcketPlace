using System.Linq.Expressions;

namespace MarcketPlace.Core.Utils;

public static class PredicatedUtils
{
    private static class Predicate<T>
    {
        public static readonly Expression<Func<T, bool>> TrueExpression = item => true;
        public static readonly Expression<Func<T, bool>> FalseExpression = item => false;
    }
    public static Expression<Func<T, bool>> Null<T>() { return null; }
    public static Expression<Func<T, bool>> True<T>() { return Predicate<T>.TrueExpression; }
    public static Expression<Func<T, bool>> False<T>() { return Predicate<T>.FalseExpression; }
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>>? left, Expression<Func<T, bool>>? right)
    {
        if (Equals(left, right)) return left;
        if (left == null || Equals(left, True<T>())) return right;
        if (right == null || Equals(right, True<T>())) return left;
        if (Equals(left, False<T>()) || Equals(right, False<T>())) return False<T>();
        var body = Expression.AndAlso(left.Body, right.Body.Replace(right.Parameters[0], left.Parameters[0]));
        return Expression.Lambda<Func<T, bool>>(body, left.Parameters);
    }
    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>>? left, Expression<Func<T, bool>>? right)
    {
        if (Equals(left, right)) return left;
        if (left == null || Equals(left, False<T>())) return right;
        if (right == null || Equals(right, False<T>())) return left;
        if (Equals(left, True<T>()) || Equals(right, True<T>())) return True<T>();
        var body = Expression.OrElse(left.Body, right.Body.Replace(right.Parameters[0], left.Parameters[0]));
        return Expression.Lambda<Func<T, bool>>(body, left.Parameters);
    }

    private static Expression Replace(this Expression expression, Expression source, Expression target)
    {
        return new ExpressionReplacer { Source = source, Target = target }.Visit(expression);
    }

    class ExpressionReplacer : ExpressionVisitor
    {
        public Expression Source = null!;
        public Expression Target = null!;

        //#pragma warning disable CS8765 // A nulidade do tipo de parâmetro não corresponde ao membro substituído (possivelmente devido a atributos de nulidade).
        public override Expression Visit(Expression? node)
        //#pragma warning restore CS8765 // A nulidade do tipo de parâmetro não corresponde ao membro substituído (possivelmente devido a atributos de nulidade).
        {
            return node == Source ? Target : base.Visit(node);
        }
    }
}