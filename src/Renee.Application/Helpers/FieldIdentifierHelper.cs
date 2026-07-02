using System.Linq.Expressions;
using Microsoft.AspNetCore.Components.Forms;

namespace Renee.Application.Helpers;

public static class FieldIdentifierHelper
{
	public static FieldIdentifier GetFieldIdentifierForProperty<TValue>(Expression<Func<TValue>> expression) =>
		FieldIdentifier.Create(expression);
}