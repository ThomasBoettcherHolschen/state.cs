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
using System.Diagnostics;

namespace Steelbreeze.Behavior
{
	/// <summary>
	/// A completion Transition between Vertices.
	/// </summary>
	public class Completion : TransitionBase
	{
		// the transitions guard condition
		internal readonly Func<Boolean> guard;

		/// <summary>
		/// The optional actions that are called while traversing the transition.
		/// </summary>
		public event Action Effect;

		/// <summary>
		/// Creates a completion transition.
		/// </summary>
		/// <param name="source">The source Vertex of the Transition.</param>
		/// <param name="target">The target Vertex of the Transition.</param>
		/// <param name="guard">An optional guard condition to restrict traversal of the transition.</param>
		public Completion( PseudoState source, Vertex target, Func<Boolean> guard = null )
			: base( source, target )
		{
			Trace.Assert( source != null, "Source PseudoState for transition must be specified." );
			Trace.Assert( target != null, "Target Vertex for completion transition must be specified." );

			this.guard = guard ?? ( () => true );

			( source.completions ?? ( source.completions = new HashSet<Completion>() ) ).Add( this );
		}

		/// <summary>
		/// Creates a completion transition.
		/// </summary>
		/// <param name="source">The source Vertex of the Transition.</param>
		/// <param name="target">The target Vertex of the Transition.</param>
		/// <param name="guard">An optional guard condition to restrict traversal of the transition.</param>
		public Completion( SimpleState source, Vertex target, Func<Boolean> guard = null )
			: base( source, target )
		{
			Trace.Assert( source != null, "Source State for transition must be specified." );
			Trace.Assert( target != null, "Target Vertex for completion transition must be specified." );

			this.guard = guard ?? ( () => true );

			( source.completions ?? ( source.completions = new HashSet<Completion>() ) ).Add( this );
		}

		internal void Traverse( IState state, Boolean deepHistory )
		{
			if( exit != null )
				exit( state );

			OnEffect();

			if( enter != null )
				enter( state );

			if( complete != null )
				complete( state, deepHistory );
		}

		/// <summary>
		/// The completion transitions behaviour
		/// </summary>
		/// <remarks>
		/// Override this method to implement more complex completion transition behaviour
		/// </remarks>
		public virtual void OnEffect()
		{
			if( Effect != null )
				Effect();
		}
	}
}