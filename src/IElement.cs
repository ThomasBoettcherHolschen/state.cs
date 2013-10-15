﻿// Copyright © 2013 Steelbreeze Limited.
// This file is part of state.cs.
//
// state.cs is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published
// by the Free Software Foundation, either version 3 of the License,
// or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;

namespace Steelbreeze.Behavior
{
	internal interface IElement
	{
		String Name { get; }
		IElement Owner { get; }

		void Exit( IState context );
		void Enter( IState context );
	}

	internal static class IElementMethods
	{
		internal static IEnumerable<IElement> Ancestors( this IElement vertex )
		{
			do
			{
				yield return vertex;
			} while( ( vertex = vertex.Owner ) != null );
		}
	}
}
