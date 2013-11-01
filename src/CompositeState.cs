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

namespace Steelbreeze.Behavior
{
	/// <summary>
	/// A composite state is a state that contains states and pseudo states.
	/// </summary>
	public class CompositeState : SimpleState
	{
		internal PseudoState Initial { private get; set; }

		/// <summary>
		/// Creates a composite state within an owning (parent) region.
		/// </summary>
		/// <param name="name">The name of the composite state.</param>
		/// <param name="owner">The owning (parent) region.</param>
		/// <remarks>
		/// A composite state is a container of states and pseudo states within a state machine model; it can be used as a root state machine.
		/// </remarks>
		public CompositeState( String name, Region owner ) : base( name, owner ) { }

		/// <summary>
		/// Creates a composite state within an owning (parent) composite state.
		/// </summary>
		/// <param name="name">The name of the composite state.</param>
		/// <param name="owner">The owning (parent) composite state.</param>
		/// <remarks>
		/// A composite state is a container of states and pseudo states within a state machine model; it can be used as a root state machine.
		/// </remarks>
		public CompositeState( String name, CompositeState owner ) : base( name, owner ) { }

		/// <summary>
		/// Tests the composite state for completeness.
		/// </summary>
		/// <param name="context">The state machine state to test.</param>
		/// <returns>True if the current state of the state machine state is a final state.</returns>
		public override bool IsComplete( IState context )
		{
			return context.IsTerminated || context.GetCurrent( this ) is FinalState;
		}

		/// <summary>
		/// Initialises the state context with its initial state.
		/// </summary>
		/// <param name="context">The state machine state context to initialise.</param>
		public void Initialise( IState context )
		{
			BeginEnter( context );
			EndEnter( context, false );
		}

		internal override void BeginExit( IState context )
		{
			var current = context.GetCurrent( this );

			if( current != null )
			{
				current.BeginExit( context );
				current.EndExit( context );
			}
		}

		internal override void EndEnter( IState context, bool deepHistory )
		{
			var current = deepHistory || this.Initial.Kind.IsHistory() ? context.GetCurrent( this ) as Element ?? this.Initial : this.Initial;

			current.BeginEnter( context );
			current.EndEnter( context, deepHistory || this.Initial.Kind == PseudoStateKind.DeepHistory );

			base.EndEnter( context, deepHistory );
		}

		/// <summary>
		/// Attempts to process a message against a composite state.
		/// </summary>
		/// <param name="context">The state machine state.</param>
		/// <param name="message">The message to evaluate.</param>
		/// <returns>A boolean indicating if the message caused a state change.</returns>
		public override Boolean Process( IState context, Object message )
		{
			if( context.IsTerminated )
				return false;

			return base.Process( context, message ) || context.GetCurrent( this ).Process( context, message );
		}
	}
}
