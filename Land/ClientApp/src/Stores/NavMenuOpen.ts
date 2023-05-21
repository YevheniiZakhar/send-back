import { makeAutoObservable } from 'mobx';

interface INavMenuOpenState {
  isOpen: boolean;
}

class NavMenuOpenStore {
  isOpen = false;

  constructor() {
    makeAutoObservable(this);
  }

  toggle() {
    this.isOpen = !this.isOpen;
    console.log(this.isOpen);
  }
}

export const navMenuOpenStore = new NavMenuOpenStore();
export type { INavMenuOpenState };
