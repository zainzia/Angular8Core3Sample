/* SystemJS module definition */

declare var module: NodeModule;

interface Stripe {
  Stripe(key: string): any;
}

interface NodeModule {
  id: string;
}
