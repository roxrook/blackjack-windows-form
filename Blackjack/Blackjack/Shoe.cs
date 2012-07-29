using System;
using System.Collections.Generic;
using System.Text;

using Cards;

namespace Blackjack {
	/// <summary>
	/// Deck-like class, containing multiple decks.
	/// </summary>
	public class Shoe : Deck {
		public Shoe(int decks)
			: base() {
			// We already contain one deck; add others:
			for (int i = 0; i < decks - 1; i++) {
				Deck d = new Deck();
				cards.AddRange(d);
			}
		}
	}
}